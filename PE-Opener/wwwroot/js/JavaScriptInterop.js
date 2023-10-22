window.createHexEditor = (elementId, bytes) => {
    console.log("JS-createHexEditor()-start");
    const target = document.getElementById(elementId);
    target.textContent = '';
    const context = new Map();
    const props = {
        height: "100%",
        width: "99%",
        data: bytes
    };

    const hexEditor = new JsHexEditor.HexEditor({ context, target, props });

    hexEditor.$set({
        bytesPerLine: 16,
    });

    window.hexEditor = hexEditor;
    console.log("JS-createHexEditor()-complete");
};

window.triggerClick = (elt) => elt.click();
