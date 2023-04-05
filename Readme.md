# HarvestHelper.Equipment

Equipment service used in HarvestHelper

## How to create and publish my package
```powershell
$version="1.0.2"
$owner="HarvestHelper" 
$gh_pat="[PAT HERE]"

dotnet pack src\HarvestHelper.Equipment.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/HarvestHelper.Equipment -o ..\packages

dotnet nuget push ..\packages\HarvestHelper.Equipment.Contracts.$version.nupkg --api-key $gh_pat --source "github" 
```


## Build the docker image
```powershell
$version="1.0.2"
$env:GH_OWNER="HarvestHelper"
$env:GH_PAT="[PAT]"
docker build --secret id=GH_OWNER --secret id=GH_PAT -t harvesthelper.equipment:$version .
```

## Run the docker image
```powershell
$version="1.0.2"
docker run -it --rm -p 5000:5000 --name equipment -e MongoDbSettings__Host=mongo -e RabbitMQSettings__Host=rabbitmq --network=harvesthelperinfra_default harvesthelper.equipment:$version
```