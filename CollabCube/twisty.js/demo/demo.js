"use strict";

var cache = window.applicationCache;
function updateReadyCache() {
  window.applicationCache.swapCache();
  location.reload(true); // For now
}

var CubeState = {
  solved: 0,
  scrambling: 1,
  scrambled: 2,
  solving: 3,
};
var cubeState = null;

var twistyScene;

$(document).ready(function() {

  cache.addEventListener('updateready', updateReadyCache, false);

  var currentCubeSize = 3;
  reloadCube();

  $("#allow_dragging").bind("change", reloadCube);
  $("#double_sided").bind("change", reloadCube);
  $("#sticker_border").bind("change", reloadCube);
  $("#cubies").bind("change", reloadCube);
  $("#hint_stickers").bind("change", reloadCube);

  $('input[name="stage"]').bind("change", reloadCube);

  $("#lucasparity").bind("click", function() {
    var lucasparity = alg.cube.stringToAlg("r U2 x r U2 r U2 r' U2 L U2 r' U2 r U2 r' U2 r'");
    twistyScene.queueMoves(lucasparity);
    twistyScene.play.start();
  });

  $('#renderer').change(reloadCube);

  $("#speed").bind("change", function() {
    var speed = $('#speed')[0].valueAsNumber
    twistyScene.setSpeed(speed);
  });

  twistyScene.setCameraPosition(0);

  function reDimensionCube() {
    var dim = 3;
    if (!dim) {
      dim = 3;
    }
    dim = Math.min(Math.max(dim, 1), 16);
    if (dim != currentCubeSize) {
      currentCubeSize = dim;
      reloadCube();
    }
  }

  // From alg.garron.us
  function escapeAlg(algstr){return algstr.replace(/\n/g, '%0A').replace(/-/g, '%2D').replace(/\'/g, '-').replace(/ /g, '_');}

  function reloadCube() {
    var renderer = THREE[$('#renderer option:selected').val() + "Renderer"]; //TODO: Unsafe
    var stage = $('input[name="stage"]:checked').val();
    var speed = $('#speed')[0].valueAsNumber;

    twistyScene = new twisty.scene({
      renderer: renderer,
      allowDragging: $("#allow_dragging").is(':checked'),
      "speed": speed,
      stats: true
    });

    $("#twistyContainer").empty();
    $("#twistyContainer").append($(twistyScene.getDomElement()));

    twistyScene.initializePuzzle({
      "type": "cube",
      "dimension": currentCubeSize,
      "stage": stage,
      "doubleSided": $("#double_sided").is(':checked'),
      "cubies": $("#cubies").is(':checked'),
      "hintStickers": $("#hint_stickers").is(':checked'),
      "stickerBorder": $("#sticker_border").is(':checked')
    });

    twistyScene.resize();
    cubeState = CubeState.solved;
  }

  $(window).resize(twistyScene.resize);

  $(window).keydown(function(e) {
    // This is kinda weird, we want to avoid turning the cube
    // if we're in a textarea, or input field.
    var focusedEl = document.activeElement.nodeName.toLowerCase();
    var isEditing = focusedEl == 'textarea' || focusedEl == 'input';
    if(isEditing) {
      return;
    }

    var keyCode = e.keyCode;
    switch(keyCode) {
      case 27:
        //reloadCube();
        e.preventDefault();
        break;

      case 32:
        //if (true) {
        //  var twisty = twistyScene.getTwisty();
        //  var scramble = twisty.generateScramble(twisty);
        //  // We're going to get notified of the scrambling, and we don't
        //  // want to start the timer when that's happening, so we keep track
        //  // of the fact that we're scrambling.
        //  cubeState = CubeState.scrambling;
        //  twistyScene.applyMoves(scramble); //TODO: Use appropriate function.
        //  cubeState = CubeState.scrambled;
        //}
        e.preventDefault();
        break;
    }

    twistyScene.keydown(e);
  });

   //twistyScene.addMoveListener(function(move, started) {
   //  if(started) {
   //    if(cubeState == CubeState.scrambling) {
   //      // We don't want to start the timer if we're scrambling the cube.
   //    } else if(cubeState == CubeState.scrambled) {
   //      var twisty = twistyScene.getTwisty();
   //      if(twisty.isInspectionLegalMove(twisty, move)) {
   //        return;
   //      }
   //      startTimer();
   //      cubeState = CubeState.solving;
   //    }
   //  } else {
   //    var twisty = twistyScene.getTwisty();
   //    if(cubeState == CubeState.solving && twisty.isSolved(twisty)) {
   //      cubeState = CubeState.solved;
   //      stopTimer();
   //    }
   //  }
   //});
});

/*
 * Algs for testing
 */

function makeCCC(n) {

  var cccMoves = [];

  for (var i = 1; i<=n/2; i++) {
    var moreMoves = [
      {base: "l", endLayer: i, amount: -1},
      {base: "u", endLayer: i, amount: 1},
      {base: "r", endLayer: i, amount: -1},
      {base: "f", endLayer: i, amount: -1},
      {base: "u", endLayer: i, amount: 1},
      {base: "l", endLayer: i, amount: -2},
      {base: "u", endLayer: i, amount: -2},
      {base: "l", endLayer: i, amount: -1},
      {base: "u", endLayer: i, amount: -1},
      {base: "l", endLayer: i, amount: 1},
      {base: "u", endLayer: i, amount: -2},
      {base: "d", endLayer: i, amount: 1},
      {base: "r", endLayer: i, amount: -1},
      {base: "d", endLayer: i, amount: -1},
      {base: "f", endLayer: i, amount: 2},
      {base: "r", endLayer: i, amount: 2},
      {base: "u", endLayer: i, amount: -1}
    ];

    cccMoves = cccMoves.concat(moreMoves);
  }
    
  return cccMoves;
}
