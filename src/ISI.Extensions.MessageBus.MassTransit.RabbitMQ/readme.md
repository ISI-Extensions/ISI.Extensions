docker run -it --name=rabbitmq -p 15672:15672 -p 5672:5672 rabbitmq:3-management

docker stop rabbitmq

docker start rabbitmq

http://localhost:15672/
