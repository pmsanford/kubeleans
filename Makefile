all: containers deploy

deploy:
	kubectl create -f Kubeleans.Deployments

containers:
	docker build -t kubeleans:main .

clean:
	kubectl delete -f Kubeleans.Deployments/kubeleans-namespace.yaml --force=true --now=true || true
	docker rmi -f kubeleans:main || true
