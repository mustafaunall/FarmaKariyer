docker container stop eucard_admin
docker container rm eucard_admin
docker image rm eucard_admin
docker build -t eucard_admin -f ./AdminUI/Dockerfile .
docker run --name eucard_admin -d -p 5300:80 eucard_admin