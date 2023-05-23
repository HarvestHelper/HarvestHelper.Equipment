# HarvestHelper.Equipment

Equipment service used in HarvestHelper

## How to create and publish my package
```powershell
$version="1.0.4"
$owner="HarvestHelper" 
$gh_pat="[PAT HERE]"

dotnet pack src\HarvestHelper.Equipment.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/HarvestHelper.Equipment -o ..\packages

dotnet nuget push ..\packages\HarvestHelper.Equipment.Contracts.$version.nupkg --api-key $gh_pat --source "github" 
```


## Build the docker image
```powershell
$version="1.0.4"
$env:GH_OWNER="HarvestHelper"
$env:GH_PAT="[PAT]"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t harvesthelper.equipment:$version .
```

## Run the docker image
### local
```powershell
$version="1.0.4"
docker run -it --rm -p 5000:5000 --name equipment -e MongoDbSettings__Host=mongo -e RabbitMQSettings__Host=rabbitmq --network=harvesthelperinfra_default harvesthelper.equipment:$version
```

### cloud
```powershell
$version="1.0.4"
$adminPass="[Password]"
$cosmosDbConnString="[Connection string]"
$serviceBusConnString="[Connection string]"
docker run -it --rm -p 5000:5000 --name equipment -e MongoDbSettings__ConnectionString=$cosmosDbConnString -e ServiceBusSettings__ConnectionString=$serviceBusConnString -e ServiceSettings__MessageBroker="SERVICEBUS" -e IdentitySettings__AdminUserPassword=$adminPass harvesthelper.equipment:$version
```

## Publishing the docker image
```powershell
$version="1.0.4"
$appname="harvesthelper"

az acr login --name $appname

docker tag harvesthelper.equipment:$version "$appname.azurecr.io/harvesthelper.equipment:$version"

docker push "$appname.azurecr.io/harvesthelper.equipment:$version"
```

## Create the kubernetes namespace
```powershell
$namespace="equipment"
kubectl create namespace $namespace
```

## Create the kubernetes pods
```powershell
$namespace="equipment"

kubectl apply -f .\kubernetes\equipment.yaml -n $namespace
```

## Create the azure managed identity and granting access to keyvault secrets
```powershell
$appname="harvesthelper"
$namespace="equipment"

az identity create --resource-group $appname --name $namespace

$IDETITY_CLIENT_ID=az identity show -g $appname -n $namespace --query clientId -otsv

az keyvault set-policy -n $appname --secret-permissions get list --spn $IDETITY_CLIENT_ID
```

## Establish the federated identity credential 
```powershell
$appname="harvesthelper"

$AKS_OIDC_ISSUER=az aks show -n $appname -g $appname --query "oidcIssuerProfile.issuerUrl" -otsv

az identity federated-credential create --name $namespace --identity-name $namespace --resource-group $appname --issuer $AKS_OIDC_ISSUER --subject "system:serviceaccount:${namespace}:${namespace}-serviceaccount"
```

### install the helm chart
```powershell
$namespace="equipment"
$appname="harvesthelper"
$helmUser=[guid]::Empty.Guid
$helmPassword=az acr login --name $appname --expose-token --output tsv --query accessToken 

helm registry login "$appname.azurecr.io" --username $helmUser --password $helmPassword

$chartVersion="0.1.0"
helm upgrade equipment-service oci://$appname.azurecr.io/helm/microservice --version $chartVersion -f .\helm\values.yaml -n $namespace --install
```

### Run SonarQube
```powershell
$sonartoken="[SONAR_TOKEN]"
dotnet sonarscanner begin /k:"HarvestHelper.Equipment" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token=$sonartoken
dotnet build
dotnet sonarscanner end /d:sonar.token=$sonartoken
```