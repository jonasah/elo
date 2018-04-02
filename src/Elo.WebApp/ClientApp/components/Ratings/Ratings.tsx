import * as React from 'react';
import { RatingsTable } from './RatingsTable';
import { LastUpdateText } from '../Common/LastUpdateText';
import { SeasonSelect } from '../Seasons/SeasonSelect';

interface RatingsProps {
    headerSize?: number;
    onlyActiveSeasons?: boolean;
}

interface RatingsState {
    lastUpdate: Date;
    selectedSeason: string;
}

export class Ratings extends React.Component<RatingsProps, RatingsState> {
    constructor(props: RatingsProps) {
        super(props);

        this.state = { lastUpdate: new Date(), selectedSeason: '' };

        this.onRatingsUpdated = this.onRatingsUpdated.bind(this);
        this.onSeasonSelected = this.onSeasonSelected.bind(this);
    }

    public render() {
        return <div>
            {this.getHeader()}
            <LastUpdateText timestamp={this.state.lastUpdate} />
            <RatingsTable season={this.state.selectedSeason} onRatingsUpdate={this.onRatingsUpdated} />
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
}
