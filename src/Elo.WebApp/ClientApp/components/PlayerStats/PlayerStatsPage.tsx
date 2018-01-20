import * as React from 'react'
import { RouteComponentProps, withRouter } from 'react-router';
import { Head2HeadRecords } from './Head2HeadRecords';
import { LatestGames } from '../Games/LatestGames';
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
                <h1>{this.player} <small>Player Stats</small></h1>
            </div>
            <div className="row">
                <div className="col-sm-6">
                    <Head2HeadRecords player={this.player} />
                    <hr />
                    <LatestGames player={this.player} numGames={10} headerSize={2} />
                </div>
                <div className="col-sm-6">
                    <ExpectedScores player={this.player} />
                </div>
            </div>
        </div>;
    }
}
