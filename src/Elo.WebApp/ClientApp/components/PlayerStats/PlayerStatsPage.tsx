import * as React from 'react'
import { RouteComponentProps, withRouter } from 'react-router';
import { Head2HeadRecords } from './Head2HeadRecords';
import { LatestGamesTable } from '../Games/LatestGamesTable';
import { ExpectedScores } from './ExpectedScores';

interface PlayerStatsPageProps {
    player: string;
}

export class PlayerStatsPage extends React.Component<RouteComponentProps<PlayerStatsPageProps>, {}> {
    player: string;

    constructor(props: RouteComponentProps<PlayerStatsPageProps>) {
        super(props);

        this.player = this.props.match.params.player;
    }

    public render() {
        return <div>
            <div className="page-header">
                <h1>Player Stats - {this.player}</h1>
            </div>
            <div className="row">
                <div className="col-sm-6">
                    <Head2HeadRecords player={this.player} />
                    <hr />
                    <h2>Latest Games</h2>
                    <LatestGamesTable player={this.player} numGames={10} />
                </div>
                <div className="col-sm-6">
                    <ExpectedScores player={this.player} />
                </div>
            </div>
        </div>;
    }
}
