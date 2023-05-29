var CustomWebGLPlugin = {


    OnLoadedUnityInstance: function () {

        document.getElementById("unity-canvas").style.display = "block";
        document.getElementById("loader").style.display = "none";
    }
};

mergeInto(LibraryManager.library, CustomWebGLPlugin);