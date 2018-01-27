import * as React from 'react';
import { LatestGamesTable } from './LatestGamesTable';

interface LatestGamesProps {
    numGames: number;
    player?: string;
    showDate?: boolean;
    showActions?: boolean;
    headerSize?: number;
}

export class LatestGames extends React.Component<LatestGamesProps, {}> {
    public render() {
        return <div>
            {this.getHeader()}
            <LatestGamesTable
                numGames={this.props.numGames}
                player={this.props.player}
                showDate={this.props.showDate}
                showActions={this.props.showActions}
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
}
