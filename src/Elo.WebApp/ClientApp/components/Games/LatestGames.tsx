import * as React from 'react';
import { LatestGamesTable } from './LatestGamesTable';
import { LastUpdateText } from '../Common/LastUpdateText';

interface LatestGamesProps {
    numGames: number;
    player?: string;
    showDate?: boolean;
    showActions?: boolean;
    headerSize?: number;
}

interface LatestGamesState {
    lastUpdate: Date;
}

export class LatestGames extends React.Component<LatestGamesProps, LatestGamesState> {
    constructor(props: LatestGamesProps) {
        super(props);

        this.state = { lastUpdate: new Date() };

        this.onGamesUpdated = this.onGamesUpdated.bind(this);
    }

    public render() {
        return <div>
            {this.getHeader()}
            <LastUpdateText timestamp={this.state.lastUpdate} />
            <LatestGamesTable
                numGames={this.props.numGames}
                player={this.props.player}
                showDate={this.props.showDate}
                showActions={this.props.showActions}
                onGamesUpdate={this.onGamesUpdated}
            />
        </div>;
    }

    getHeader() {
        switch (this.props.headerSize) {
            case 2:
                return <h2>Latest Games</h2>;
            default:
                return <h1>Latest Games</h1>;
        }
    }

    onGamesUpdated() {
        this.setState({ lastUpdate: new Date() });
    }
}
