docker container stop eucard
docker container rm eucard
docker image rm eucard
docker build -t eucard -f ./WebUI/Dockerfile .
docker run --name eucard -d -p 5200:80 eucard