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
    scenefile: './samples/materials/scene.json',
}, './threejs-materials.html');
writeExamplePage({
    scenefile: './samples/models/scene.json',
}, './threejs-models.html');
writeExamplePage({
    scenefile: './samples/script-variables/scene.json',
}, './threejs-script-variables.html');
writeExamplePage({
    scenefile: './samples/simple-scene/scene.json',
}, './threejs-simple-scene.html');

function writeIndexPage() {
    ejs.renderFile('./views/pages/index.ejs', {}, {}, function(err, str) {
        fs.writeFile('./index.html', str, function(err) {
        });
    });
}
writeIndexPage();