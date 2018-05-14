var renderer = new THREE.WebGLRenderer();
var scene = new THREE.Scene();
var sphere = new THREE.Mesh(new THREE.SphereGeometry(20, 8, 8), new THREE.MeshNormalMaterial());
var camera = new THREE.PerspectiveCamera(50, window.innerWidth / window.innerHeight, 1, 1000);
var myAnimationFrame;
var webglActive = webgl_detect(1);
var drawingElement;
var increment = true;
var startZ = 60;
var endZ = 300;

function BeginAnimation(drawingElementId, width, height) {

    DoAnimation(drawingElementId, width, height);

}
function BeginAnimationAuto(elementId) {

    var element = document.getElementById(elementId);
    //element.offsetWidth
    DoAnimation(elementId, 500, 500);

}

function EndAnimation() {

    cancelAnimationFrame(myAnimationFrame);
    scene.remove(sphere);
    drawingElement.removeChild(renderer.domElement);
}

function InitGlobalObjects(drawLocation, width, height) {

    renderer.setSize(width, height);
    drawingElement = document.getElementById(drawLocation);
    drawingElement.append(renderer.domElement);


    //$(drawLocation).append(renderer.domElement);

    //document.body.appendChild(renderer.domElement);
    // camera
    camera.position.z = startZ;

    sphere.overdraw = true;
    sphere.x = 0;
    sphere.y = 0;

    scene.add(sphere);

    renderer.render(scene, camera);
}


function DoAnimation(drawLocation, width, height) {

    if (webglActive) {
        InitGlobalObjects(drawLocation, width, height);
        render();
    }
}

function render() {

    myAnimationFrame = requestAnimationFrame(render);

    if (camera.position.z >= endZ) {
        increment=false
    }
    else if (camera.position.z <= startZ) {
        increment = true;
    }
    IncreaseOrDecrease();
    renderer.render(scene, camera);
}

function IncreaseOrDecrease() {
    if (increment == true) {
        sphere.rotation.x += 0.1;
        sphere.rotation.y += 0.1;
        camera.position.z += 1;

    }
    else {
        sphere.rotation.x -= 0.1;
        sphere.rotation.y -= 0.1;
        camera.position.z -= 1;
    }

}

function webgl_detect(return_context) {
    if (!!window.WebGLRenderingContext) {
        var canvas = document.createElement("canvas"),
             names = ["webgl", "experimental-webgl", "moz-webgl", "webkit-3d"],
           context = false;

        for (var i = 0; i < 4; i++) {
            try {
                context = canvas.getContext(names[i]);
                if (context && typeof context.getParameter == "function") {
                    // WebGL is enabled
                    if (return_context) {
                        // return WebGL object if the function's argument is present
                        return { name: names[i], gl: context };
                    }
                    // else, return just true
                    return true;
                }
            } catch (e) { }
        }

        // WebGL is supported, but disabled
        return false;
    }

    // WebGL not supported
    return false;
}
