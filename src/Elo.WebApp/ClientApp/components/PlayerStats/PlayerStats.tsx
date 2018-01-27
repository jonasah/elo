import * as React from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Head2HeadRecords } from './Head2HeadRecords';
import { LatestGames } from '../Games/LatestGames';
import { ExpectedScores } from './ExpectedScores';

interface PlayerStatsProps {
    player: string;
}

export class PlayerStats extends React.Component<RouteComponentProps<PlayerStatsProps>, {}> {
    constructor(props: RouteComponentProps<PlayerStatsProps>) {
        super(props);
    }

    public render() {
        return <div>
            <div className="page-header">
                <h1>{this.props.match.params.player} <small>Player Stats</small></h1>
            </div>
            <div className="row">
                <div className="col-sm-6">
                    <Head2HeadRecords player={this.props.match.params.player} />
                    <hr />
                    <LatestGames
                        player={this.props.match.params.player}
                        numGames={10}
                        showActions={false}
                        headerSize={2}
                    />
                </div>
                <div className="col-sm-6">
                    <ExpectedScores player={this.props.match.params.player} />
                </div>
            </div>
        </div>;
    }
}
