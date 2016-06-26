function findByUserData(node, key, value) {
    if(node.userData && node.userData[key] == value) {
        return node;
    }

    for(var i = 0 ; i < node.children.length ; i++) {
        var child = node.children[i];
        var found = findByUserData(child, key, value);
        if(found != null) {
            return found;
        }
    }

    return undefined;
}

