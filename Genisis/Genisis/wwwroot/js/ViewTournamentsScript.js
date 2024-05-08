let searchDebounceTimer;

function toggleAccordion(panelIndex) {
    var panel = document.getElementById(`accordion-panel-${panelIndex}`);
    if (panel.style.display === "block") {
        panel.style.display = "none";
    } else {
        panel.style.display = "block";
        panel.style.height = "auto";
    }
}

function SearchForTournament(searchTextBox) {
    var searchValue = searchTextBox.value.toLowerCase();

    var tournamentDivs = document.querySelectorAll('.accordion-body');
    tournamentDivs.forEach(function (div) {
        div.style.display = 'none';
    });

    if (searchValue === '') {
        tournamentDivs.forEach(function (div) {
            div.style.display = 'block';
        });
    } else {
        tournamentDivs.forEach(function (div) {
            var tournamentName = div.getAttribute('data-tournament-name').toLowerCase();
            if (tournamentName.includes(searchValue)) {
                div.style.display = 'block';
            }
        });
    }
}