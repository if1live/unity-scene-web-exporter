#!/bin/bash
# reference : https://tintypemolly.github.io/pelican-setup.html

function clear_previous_content() {
    rm -rf documents
    rm -rf libs
    rm -rf sample-aframe 
    rm -rf sample-threejs
    rm -rf *.html 
    rm -rf *.js 
    rm -rf *.css 
}

function copy_content() {
    cp -r documents output
    cp -r libs output
    cp -r sample-aframe output
    cp -r sample-threejs output
    cp *.html output
    cp *.js output
    cp *.css output
}

function publish() {
	SHA=`git rev-parse --verify HEAD`
    git add .
	git commit -a -m "Deploy to GitHub Pages: ${SHA}"
    if [[ $? == 0 ]]; then
        git push origin gh-pages
    fi
}

function configure_ssh() {
	git config --global user.email "libsora25@gmail.com"
	git config --global user.name "Travis"
	
	ENCRYPTED_KEY_VAR="encrypted_${ENCRYPTION_LABEL}_key"
	ENCRYPTED_IV_VAR="encrypted_${ENCRYPTION_LABEL}_iv"
	openssl aes-256-cbc -K $encrypted_60ccbfd98931_key -iv $encrypted_60ccbfd98931_iv -in deploy_key.enc -out deploy_key -d
	chmod 600 deploy_key
	eval `ssh-agent -s`
	ssh-add deploy_key
}

configure_ssh;

cd SimpleViewer
node index.js

git clone --depth 1 --quiet -b gh-pages git@github.com:if1live/unity-scene-web-exporter.git output

cd output; clear_previous_content; cd ..

copy_content;

cd output; publish;

cd ..


