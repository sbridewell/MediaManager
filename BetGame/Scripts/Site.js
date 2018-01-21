$(document).ready(function () {
    //alert("ready");

    // How long is the racecourse in arbitrary units?
    var lengthOfRacecourse = 18;

    // How many horses are in the race?
    var numberOfHorses = 5;

    // zero-based index of the horse which has won the race, or -1 if nobody has won yet
    var winner = -1;

    // True if the race is being run, false if it hasn't started or has finished
    var raceInProgress = false;

    // How much money does the player have to gamble with?
    var balance = 50;

    // Where are the horses? (index into the laneXPositions array)
    var horsePositions = [4, 3, 2, 1, 0];

    // vertical co-ordinates of each lane in the racecourse
    var laneYPosition = [30, 40, 50, 60, 70];

    // horizontal co-ordinates of the positions a horse can occupy within the lane
    // index 0 is the starting line, index 18 is the finish line
    var laneXPosition = [5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85, 90, 95];

    refreshBalance();
    addHorsesToRacecourse();
    newRace();
    setEventHandlers();

    // Sets event handlers for the buttons and any other controls.
    function setEventHandlers() {
        $('#buttonNewRace').click(function () {
            newRace();
        });

        $('#buttonStartRace').click(function () {
            startRace();
        });

        $('#buttonNewGame').click(function () {
            location.reload();
        })
    }

    // Adds elements representing the horses to the DOM.
    function addHorsesToRacecourse() {
        for (i = 0; i < numberOfHorses; i++) {
            var glyphicon = '<div class="glyphicon glyphicon-chevron-right"></div>';
            var horseText = i + 1 + glyphicon;
            var horse = '<div id="horse' + i + '" class="horse">' + horseText + '</div>';
            $('#racecourse').append(horse);
        }
    }

    // Initialises a new race but doesn't start it yet.
    // The user needs to pick a horse and an amount to bet before starting the race.
    function newRace() {
        raceInProgress = false;
        horsePositions = [4, 3, 2, 1, 0];
        moveHorses();
        $('#buttonNewRace').hide();
        $('#buttonStartRace').show();
        $('#buttonNewGame').hide();
        $('#horseToBack').prop('disabled', false).removeClass('disabled');
        $('#valueToBet').prop('disabled', false).removeClass('disabled');
        $('#youreSkint').hide();
        $('#millionaire').hide();
        $('#results').hide();
    }

    // Starts the race.
    function startRace() {
        if ($('#valueToBet').val() > balance)
        {
            alert('You don\'t have that much money!');
            return;
        }

        $('#horseToBack').prop('disabled', true).addClass('disabled');
        $('#valueToBet').prop('disabled', true).addClass('disabled');
        $('#buttonStartRace').hide();
        raceInProgress = true;
        winner = -1;
        moveHorses();

        // rollTheDice will call moveHorses, which in turn will call rollTheDice 
        // until the race is finished
        rollTheDice();
    }

    // Rolls an imaginary dice to decide which horse gets to move forward,
    // and then moves the horses to their new positions.
    function rollTheDice() {
        var diceRoll = Math.floor(Math.random() * numberOfHorses);
        horsePositions[diceRoll]++;
        moveHorses();
    }

    // Moves the horses' DOM elements to represent their current positions.
    // Tests whether one of the horses has won yet, and if so calls the endRace function.
    function moveHorses() {
        for (var i = 0; i < numberOfHorses; i++) {
            var horsePosition = horsePositions[i];
            var screenPosition = mapGridToScreen(
                laneXPosition[horsePosition],
                laneYPosition[i]);
            var horse = $('#horse' + i);
            horse.animate(screenPosition).promise().done(function () {
                if (raceInProgress == true) {
                    // has one of the horses reached the finish line?
                    for (var i = 0; i < numberOfHorses; i++) {
                        if (horsePositions[i] == lengthOfRacecourse) {
                            winner = i;
                        }
                    }

                    if (winner != -1) {
                        endRace();
                    }
                    else {
                        rollTheDice();
                    }
                }
            });
            horse.length;
        }
    }

    // Perfoms UI updates once the race has finished.
    function endRace() {
        raceInProgress = false;
        var backedHorse = Number($('#horseToBack').val());
        var resultsText = 'The winner was ' + (winner + 1)
            + ' and you backed ' + (backedHorse + 1) + '. ';

        var valueToBet = $('#valueToBet').val();
        if (backedHorse == winner) {
            var winnings = valueToBet * (winner + 1);
            balance += winnings;
            resultsText += 'You won £' + winnings;
        } else {
            balance -= valueToBet;
            resultsText += 'You lost £' + valueToBet;
        }

        refreshBalance();
        resultsText += '.';
        $('#results').text(resultsText).show();

        if (balance <= 0) {
            $('#youreSkint').show();
            $('#buttonNewGame').show();
        } else if (balance >= 1000000) {
            $('#millionaire').show();
            $('#buttonNewGame').show();
        }
        else {
            $('#buttonNewRace').show();
        }
    }

    // Converts co-ordinates in a 100x100 grid to actual screen co-ordinates.
    function mapGridToScreen(x, y) {
        var screenWidth = $(window).width();
        var screenHeight = $(window).height();
        var screenX = x * screenWidth / 100;
        var screenY = y * screenHeight / 100;
        return {
            left: screenX,
            top: screenY
        };
    }

    // Updates the player's current balance in the UI.
    function refreshBalance() {
        $('#playersMoney').text(balance);
    }
});