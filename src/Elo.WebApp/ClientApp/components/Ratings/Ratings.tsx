import * as React from 'react';
import { RatingsTable } from './RatingsTable';
import { LastUpdateText } from '../Common/LastUpdateText';
import { SeasonSelect } from '../Seasons/SeasonSelect';

interface RatingsProps {
    headerSize?: number;
    onlyActiveSeasons?: boolean;
    enablePlayerFilter?: boolean;
}

interface RatingsState {
    lastUpdate: Date;
    selectedSeason: string;
    playerFilterActive: boolean;
}

export class Ratings extends React.Component<RatingsProps, RatingsState> {
    constructor(props: RatingsProps) {
        super(props);

        this.state = {
            lastUpdate: new Date(),
            selectedSeason: '',
            playerFilterActive: props.enablePlayerFilter === true
        };

        this.onRatingsUpdated = this.onRatingsUpdated.bind(this);
        this.onSeasonSelected = this.onSeasonSelected.bind(this);
        this.togglePlayerFilter = this.togglePlayerFilter.bind(this);
    }

    public render() {
        return <div className="row">
            <div className="col-sm-12">
                {this.getHeader()}
            </div>
            <div className="col-sm-6">
                <LastUpdateText timestamp={this.state.lastUpdate} />
            </div>
            <div className="col-sm-6 text-right">
                {this.props.enablePlayerFilter === true &&
                    <div className="checkbox-inline">
                        <label className="show-all-players-label">
                            <input type="checkbox" onChange={this.togglePlayerFilter} />Show all players
                        </label>
                    </div>
                }
            </div>
            <div className="col-sm-12">
                <RatingsTable season={this.state.selectedSeason} onRatingsUpdate={this.onRatingsUpdated} playerFilterActive={this.state.playerFilterActive} />
            </div>
        </div>;
    }

    getHeader() {
        switch (this.props.headerSize) {
            case 2:
                return <h2>Ratings {this.getSeasonSelect()}</h2>;
            default:
                return <h1>Ratings {this.getSeasonSelect()}</h1>;
        }
    }

    getSeasonSelect() {
        return <SeasonSelect
            selectedSeason={this.state.selectedSeason}
            onSeasonSelected={this.onSeasonSelected}
            onlyActiveSeasons={this.props.onlyActiveSeasons}
        />;
    }

    onRatingsUpdated() {
        this.setState({ lastUpdate: new Date() });
    }

    onSeasonSelected(season: string) {
        this.setState({ selectedSeason: season });
    }

    togglePlayerFilter() {
        this.setState((prevState: RatingsState, props: RatingsProps) => {
            return { playerFilterActive: !prevState.playerFilterActive }
        });
    }
}
