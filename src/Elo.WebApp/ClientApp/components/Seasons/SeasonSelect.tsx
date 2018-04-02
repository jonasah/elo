import * as React from 'react'
import * as Api from '../../api'

interface SeasonSelectProps {
    selectedSeason: string;
    onSeasonSelected: (name: string) => void;
    onlyActiveSeasons?: boolean;
    player?: string;
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
        this.fetchSeasons(this.props);
    }

    componentWillReceiveProps(nextProps: Readonly<SeasonSelectProps>) {
        if (nextProps.player != this.props.player) {
            this.setState({ seasons: [] });
            this.fetchSeasons(nextProps);
        }
    }

    fetchSeasons(props: SeasonSelectProps) {
        var apiCall = (props.onlyActiveSeasons === true ? Api.getActiveSeasons : Api.getStartedSeasons);

        apiCall(props.player)
            .then(data => {
                this.setState({ seasons: data });
                props.onSeasonSelected(data[data.length - 1]);
            });
    }

    isSelectedSeason(season: string) {
        return season === this.props.selectedSeason;
    }
}