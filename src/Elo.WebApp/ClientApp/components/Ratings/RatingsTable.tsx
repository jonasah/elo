import * as React from 'react'
import { Link } from 'react-router-dom'
import { PlayerStatsLink } from '../Common/PlayerStatsLink';
import * as Api from '../../api';

interface RatingsTableProps {
    season: string;
    onRatingsUpdate?: () => void;
    playerFilterActive: boolean;
}

interface RatingsTableState {
    ratings: Api.Models.Rating[];
}

export class RatingsTable extends React.Component<RatingsTableProps, RatingsTableState> {
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
                    <th className="text-center"><abbr title="Change from last game in parenthesis">Rating</abbr></th>
                    <th className="text-center">Games</th>
                    <th className="text-center">Win</th>
                    <th className="text-center">Loss</th>
                    <th className="text-center">Pct</th>
                    <th className="text-center"><abbr title="Current win/loss streak">Streak</abbr></th>
                </tr>
            </thead>
            <tbody>
                {this.state.ratings.map(rating =>
                    <tr key={rating.id}>
                        <td className="text-center">{rating.rank}</td>
                        <td className="text-center">
                            <PlayerStatsLink player={rating.player}/>
                        </td>
                        <td className="text-center">
                            {rating.rating}
                            &nbsp;
                            {this.getRatingChange(rating.ratingChange)}
                        </td>
                        <td className="text-center">{rating.gamesPlayed}</td>
                        <td className="text-center">{rating.wins}</td>
                        <td className="text-center">{rating.losses}</td>
                        <td className="text-center">{(100 * rating.pct).toFixed(1)}</td>
                        <td className="text-center">{this.getStreakString(rating.streak)}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }

    fetchRatings(props: RatingsTableProps) {
        if (props.season == '') {
            return;
        }

        Api.getRatings(props.season, props.playerFilterActive ? 5 : 1)
            .then(data => {
                this.setState({ ratings: data });

                if (props.onRatingsUpdate !== undefined) {
                    props.onRatingsUpdate();
                }
            });
    }

    getRatingChange(ratingChange: number) {
        var ratingChangeString = (ratingChange > 0 ? `+${ratingChange}` : `${ratingChange}`);
        var textClass = (ratingChange > 0 ? "text-success" : (ratingChange < 0 ? "text-danger" : ""));

        return <small>(<span className={textClass}>{ratingChangeString}</span>)</small>;
    }

    getStreakString(streak: number) {
        // positive: winning streak, negative: losing streak
        return (streak > 0 ? `W${streak}` : `L${-streak}`);
    }

    componentWillMount() {
        this.fetchRatings(this.props);
    }

    componentDidMount() {
        this.timerId = setInterval(() => this.fetchRatings(this.props), 3000);
    }

    componentWillUnmount() {
        clearInterval(this.timerId);
    }

    componentWillReceiveProps(nextProps: Readonly<RatingsTableProps>) {
        if (this.props.season != nextProps.season || this.props.playerFilterActive != nextProps.playerFilterActive) {
            this.setState({ ratings: [] });
            this.fetchRatings(nextProps);
        }
    }
}
