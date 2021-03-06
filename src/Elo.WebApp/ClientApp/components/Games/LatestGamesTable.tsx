﻿import * as React from 'react'
import { Link } from 'react-router-dom';
import { PlayerStatsLink } from '../Common/PlayerStatsLink';
import * as Api from '../../api';

interface GamesTableState {
    games: Api.Models.Game[];
    deleteDisabled: boolean;
}

interface GamesTableProps {
    numGames: number;
    player?: string;
    showDate?: boolean;
    showActions?: boolean;
    onGamesUpdate?: () => void;
}

export class LatestGamesTable extends React.Component<GamesTableProps, GamesTableState> {
    timerId: number;

    constructor(props: GamesTableProps) {
        super(props);

        this.state = { games: [], deleteDisabled: false };
    }

    public render() {
        return <div>
            <table className="table table-condensed table-striped table-hover table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Winner</th>
                        <th className="text-center">Loser</th>
                        {this.props.showDate !== false &&
                            <th className="text-center">Date</th>
                        }
                        {this.props.showActions !== false &&
                            <th className="text-center">Actions</th>
                        }
                    </tr>
                </thead>
                <tbody>
                    {this.state.games.map(game =>
                        <tr key={game.id}>
                            <td className="text-center">
                                {this.props.player === game.winner && game.winner}
                                {this.props.player !== game.winner &&
                                    <PlayerStatsLink player={game.winner} />
                                }
                            </td>
                            <td className="text-center">
                                {this.props.player === game.loser && game.loser}
                                {this.props.player !== game.loser &&
                                    <PlayerStatsLink player={game.loser} />
                                }
                            </td>
                            {this.props.showDate !== false &&
                                <td className="text-center">{new Date(game.date).toLocaleString()}</td>
                            }
                            {this.props.showActions !== false &&
                                <td className="text-center">
                                <button className="btn btn-default btn-xs" type="button" onClick={() => this.deleteGame(game.id)} disabled={!game.canBeDeleted || this.state.deleteDisabled}>Delete</button>
                                </td>
                            }
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;
    }

    fetchGames(props: GamesTableProps) {
        Api.getLatestGames(props.numGames, props.player)
            .then(data => {
                this.setState({ games: data })

                if (this.props.onGamesUpdate !== undefined) {
                    this.props.onGamesUpdate();
                }
            });
    }

    deleteGame(id: number) {
        this.setState({ deleteDisabled: true });

        Api.deleteGame(id)
            .then(data => {
                this.setState({ deleteDisabled: false });
                this.fetchGames(this.props);
            });
    }

    componentWillMount() {
        this.fetchGames(this.props);
    }

    componentDidMount() {
        this.timerId = setInterval(() => this.fetchGames(this.props), 10*1000);
    }

    componentWillReceiveProps(nextProps: Readonly<GamesTableProps>) {
        if (this.props.player != nextProps.player) {
            this.setState({ games: [] });
            this.fetchGames(nextProps);
        }
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }
}
