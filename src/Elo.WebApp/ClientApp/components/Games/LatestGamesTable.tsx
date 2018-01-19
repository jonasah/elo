import * as React from 'react'
import { Link } from 'react-router-dom';
import { PlayerStatsLink } from '../Common/PlayerStatsLink';
import * as Api from '../../api';

interface GamesTableState {
    games: Api.Models.Game[];
}

interface GamesTableProps {
    numGames: number;
    player?: string;
    showDate?: boolean;
}

export class LatestGamesTable extends React.Component<GamesTableProps, GamesTableState> {
    timerId: number;

    constructor(props: GamesTableProps) {
        super(props);

        this.state = { games: [] };

        this.fetchGames();
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
                                <td className="text-center">{game.date}</td>
                            }
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;
    }

    fetchGames() {
        Api.getLatestGames(this.props.numGames, this.props.player)
            .then(data => this.setState({ games: data }));
    }

    componentDidMount() {
        this.timerId = setInterval(() => this.fetchGames(), 30*1000);
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }
}
