all: containers deploy

deploy:
	kubectl create -f Kubeleans.Deployments

containers: app silo

silo:
	docker build -f Silo.Dockerfile -t kubeleans:silo .

app:
	docker build -f Client.Dockerfile -t kubeleans:client .

clean:
	kubectl delete -f Kubeleans.Deployments/kubeleans-namespace.yaml --force=true --now=true || true
	docker rmi -f kubeleans:silo || true
	docker rmi -f kubeleans:client || true
