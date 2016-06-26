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
    scenefile: './materials/scene.json',
}, './samples/materials.html');
writeExamplePage({
    scenefile: './models/scene.json',
}, './samples/models.html');
writeExamplePage({
    scenefile: './script-variables/scene.json',
}, './samples/script-variables.html');
writeExamplePage({
    scenefile: './simple-scene/scene.json',
}, './samples/simple-scene.html');

function writeIndexPage() {
    ejs.renderFile('./views/pages/index.ejs', {}, {}, function(err, str) {
        fs.writeFile('./index.html', str, function(err) {
        });
    });
}
writeIndexPage();