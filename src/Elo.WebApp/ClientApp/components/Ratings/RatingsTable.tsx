﻿import * as React from 'react'
import { Link } from 'react-router-dom'

interface PlayerRatingDto {
    id: number;
    rank: number;
    player: string;
    rating: number;
    gamesPlayed: number;
    wins: number;
    losses: number;
    pct: number;
}

interface RatingsTableState {
    ratings: PlayerRatingDto[];
}

export class RatingsTable extends React.Component<{}, RatingsTableState> {
    timerId: number;

    constructor() {
        super();
        this.state = { ratings: [] };
    }

    public render() {
        return <table className="table table-condensed table-striped table-hover table-bordered">
            <thead>
                <tr>
                    <th className="text-center">Rank</th>
                    <th className="text-center">Player</th>
                    <th className="text-center">Rating</th>
                    <th className="text-center">Games</th>
                    <th className="text-center">Wins</th>
                    <th className="text-center">Losses</th>
                    <th className="text-center">Pct</th>
                </tr>
            </thead>
            <tbody>
                {this.state.ratings.map(rating =>
                    <tr key={rating.id}>
                        <td className="text-center">{rating.rank}</td>
                        <td className="text-center">
                            <Link to={'/playerstats/' + rating.player}>
                                {rating.player}
                            </Link>
                        </td>
                        <td className="text-center">{rating.rating}</td>
                        <td className="text-center">{rating.gamesPlayed}</td>
                        <td className="text-center">{rating.wins}</td>
                        <td className="text-center">{rating.losses}</td>
                        <td className="text-center">{(100*rating.pct).toFixed(1)}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }

    fetchRatings() {
        fetch('api/elo/ratings')
            .then(response => response.json() as Promise<PlayerRatingDto[]>)
            .then(data => {
                this.setState({ ratings: data });
            });
    }

    componentDidMount() {
        this.timerId = setInterval(() => this.fetchRatings(), 1000);
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }
}