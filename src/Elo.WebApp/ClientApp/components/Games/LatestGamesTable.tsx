import * as React from 'react'
import { Link } from 'react-router-dom';
import { PlayerStatsLink } from '../Common/PlayerStatsLink';

interface GameDto {
    id: number;
    winner: string;
    loser: string;
    date: string;
}

interface GamesTableState {
    games: GameDto[];
}

interface GamesTableProps {
    numGames: number;
    player?: string;
    showDate?: boolean;
}

export class LatestGamesTable extends React.Component<GamesTableProps, GamesTableState> {
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
        var requestUrl = 'api/elo/games';

        if (this.props.player !== undefined) {
            requestUrl += '/' + this.props.player;
        }

        fetch(requestUrl + '?page=1&pageSize=' + this.props.numGames)
            .then(response => response.json() as Promise<GameDto[]>)
            .then(data => this.setState({ games: data }));
    }
}
