docker run -p 8081:8081 -p 10251:10251 -p 10252:10252 -p 10253:10253 -p 10254:10254 --name=cosmos -it mcr.microsoft.com/cosmosdb/linux/azure-cosmos-emulator

docker stop cosmos

docker start cosmos
