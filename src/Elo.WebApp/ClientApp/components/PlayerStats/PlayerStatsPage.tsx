﻿import * as React from 'react'
import { RouteComponentProps, withRouter } from 'react-router';
import { Head2HeadRecords } from './Head2HeadRecords';
import { LatestGames } from '../Games/LatestGames';
import { ExpectedScores } from './ExpectedScores';
import { PlayerStatsLink } from '../Common/PlayerStatsLink';
import * as Api from '../../api';

interface PlayerStatsPageProps {
    player?: string;
}

interface PlayerStatsPageState {
    players: string[];
}

export class PlayerStatsPage extends React.Component<RouteComponentProps<PlayerStatsPageProps>, PlayerStatsPageState> {
    constructor(props: RouteComponentProps<PlayerStatsPageProps>) {
        super(props);

        this.state = { players: [] };

        Api.getPlayers().then(data => this.setState({ players: data }));
    }

    public render() {
        return <div>
            <div className="dropdown">
                <button className="btn btn-default dropdown-toggle" type="button" data-toggle="dropdown">
                    Select player <span className="caret"/>
                </button>
                <ul className="dropdown-menu">
                    {this.state.players.map(player =>
                        <li key={player}>
                            <PlayerStatsLink player={player} />
                        </li>
                    )}
                </ul>
            </div>
            {this.props.match.params.player !== undefined &&
                <div>
                    <div className="page-header">
                        <h1>{this.props.match.params.player} <small>Player Stats</small></h1>
                    </div>
                    <div className="row">
                        <div className="col-sm-6">
                            <Head2HeadRecords player={this.props.match.params.player} />
                            <hr />
                            <LatestGames player={this.props.match.params.player} numGames={10} headerSize={2} />
                        </div>
                        <div className="col-sm-6">
                            <ExpectedScores player={this.props.match.params.player} />
                        </div>
                    </div>
                </div>
            }
        </div>;
    }
}
