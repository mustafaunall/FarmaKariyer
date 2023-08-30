docker container stop eucard
docker container rm eucard
docker image rm eucard
docker build -t eucard -f ./WebUI/Dockerfile .
docker run --name eucard -d -p 5200:80 eucard
echo "Hazır => https://eucard.munal.net"

docker container stop eucard_admin
docker container rm eucard_admin
docker image rm eucard_admin
docker build -t eucard_admin -f ./AdminUI/Dockerfile .
docker run --name eucard_admin -d -p 5300:80 eucard_admin
echo "Hazır => https://eucardpanel.munal.net"
