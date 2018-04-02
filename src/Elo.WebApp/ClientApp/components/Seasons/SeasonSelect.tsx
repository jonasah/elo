import * as React from 'react'
import * as Api from '../../api'

interface SeasonSelectProps {
    selectedSeason: string;
    onSeasonSelected: (name: string) => void;
    onlyActiveSeasons?: boolean;
}

interface SeasonSelectState {
    seasons: string[];
}

export class SeasonSelect extends React.Component<SeasonSelectProps, SeasonSelectState> {
    constructor() {
        super();

        this.state = { seasons: [] };
        this.onButtonClicked = this.onButtonClicked.bind(this);
    }

    public render() {
        return <div className="btn-group btn-group-xs" role="group">
            {this.state.seasons.map(season =>
                <button
                    type="button"
                    className={"btn btn-default" + (this.isSelectedSeason(season) ? " active" : "")}
                    onClick={this.onButtonClicked}
                    key={season}
                >{season}</button>
            )}
        </div>;
    }

    onButtonClicked(e: React.MouseEvent<HTMLButtonElement>) {
        this.props.onSeasonSelected(e.currentTarget.innerText);
    }

    componentWillMount() {
        var apiCall = (this.props.onlyActiveSeasons === true ? Api.getActiveSeasons : Api.getStartedSeasons);

        apiCall()
            .then(data => {
                this.setState({ seasons: data });
                this.props.onSeasonSelected(data[data.length - 1]);
            });
    }

    isSelectedSeason(season: string) {
        return season === this.props.selectedSeason;
    }
}