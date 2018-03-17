#!/bin/sh

DEPLOY_HOT_RELOADER=false

PROJECT_NAME=Alien

PROJECT_DIR=`dirname $0`
cd ${PROJECT_DIR} # in case this is executed from somewhere else

TARGET=../game/Mods/${PROJECT_NAME}

rm -rf ${TARGET}

mkdir ${TARGET}

rsync -av --progress src/* "${TARGET}"  \
	--exclude Assemblies \
	--exclude */bin \
	--exclude */obj  \

mkdir ${TARGET}/Assemblies

cp -v src/Assemblies/${PROJECT_NAME}.dll ${TARGET}/Assemblies

if [ ${DEPLOY_HOT_RELOADER} = true ] ; then
    cp -v src/Assemblies/0Reloader.dll ${TARGET}/Assemblies
fi
