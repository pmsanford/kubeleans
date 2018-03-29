#!/bin/bash
docker build -f Client.Dockerfile -t kubeleans:client .
docker build -f Silo.Dockerfile -t kubeleans:silo .
