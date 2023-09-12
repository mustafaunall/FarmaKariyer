docker container stop farma
docker container rm farma
docker image rm farma
docker build -t farma -f ./WebUI/Dockerfile .
docker run --name farma -d -p 6100:80 farma
echo "Hazır => https://farmakariyer.munal.net"

# docker container stop farma_admin
# docker container rm farma_admin
# docker image rm farma_admin
# docker build -t farma_admin -f ./AdminUI/Dockerfile .
# docker run --name farma_admin -d -p 5300:80 farma_admin
# echo "Hazır => https://farmakariyerpanel.munal.net"
