Thiis project needs a RabbitMq runing to work:

docker pull heidiks/rabbitmq-delayed-message-exchange

docker run -d --name rabbitmq -p 8080:15672 -p 5672:5672 -p 25676:25676 --hostname rabbitmq-master -v /docker/rabbitmq/data:/var/lib/rabbitmq heidiks/rabbitmq-delayed-message-exchange:latest
