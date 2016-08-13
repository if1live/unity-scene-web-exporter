const ejs = require('ejs');
const fs = require('fs');

function writeExamplePage(data, output) {
    const baseTemplateFile = './views/pages/layout.ejs';
    ejs.renderFile(baseTemplateFile, data, {}, function(err, str) {
        fs.writeFile(output, str, function(err) {
            if(err) {
                return console.error("cannot write file : " + output);
            }
        });      
    });
}

writeExamplePage({
    scenefile: './sample-threejs/materials/scene.json',
}, './threejs-materials.html');
writeExamplePage({
    scenefile: './sample-threejs/models/scene.json',
}, './threejs-models.html');
writeExamplePage({
    scenefile: './sample-threejs/script-variables/scene.json',
}, './threejs-script-variables.html');
writeExamplePage({
    scenefile: './sample-threejs/simple-scene/scene.json',
}, './threejs-simple-scene.html');
writeExamplePage({
    scenefile: './sample-threejs/5minlab/scene.json',
}, './threejs-5minlab-scene.html');

function writeIndexPage() {
    ejs.renderFile('./views/pages/index.ejs', {}, {}, function(err, str) {
        fs.writeFile('./index.html', str, function(err) {
        });
    });
}
writeIndexPage();