docker container stop farma
docker container rm farma
docker image rm farma
docker build -t farma -f ./WebUI/Dockerfile .
docker run --name farma -d -p 6100:80 -e TZ=Europe/Istanbul farma
echo "Hazır => https://farmakariyer.net"

curl -H "Title: Docker Bildirimi" -d "FarmaKariyer dockerlandı." https://ntfy.cloudnoyo.com/business