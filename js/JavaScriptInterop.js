window.createHexEditor = (elementId, bytes) => {
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
};

window.triggerClick = (elt) => elt.click();
