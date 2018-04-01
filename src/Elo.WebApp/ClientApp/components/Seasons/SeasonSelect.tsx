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
        if (this.props.onlyActiveSeasons === true) {
            Api.getActiveSeasons().then(data => this.setState({ seasons: data }));
        }
        else {
            Api.getStartedSeasons().then(data => this.setState({ seasons: data }));
        }
    }

    isSelectedSeason(season: string) {
        return season === this.props.selectedSeason;
    }
}