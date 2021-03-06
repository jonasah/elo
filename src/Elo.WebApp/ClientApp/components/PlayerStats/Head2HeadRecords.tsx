﻿import * as React from 'react';
import { Link } from 'react-router-dom';
import { PlayerStatsLink } from '../Common/PlayerStatsLink';
import * as Api from '../../api';

interface Head2HeadRecordsProps {
    player: string;
    season: string;
}

interface Head2HeadRecordsState {
    records: Api.Models.Head2HeadRecord[];
}

export class Head2HeadRecords extends React.Component<Head2HeadRecordsProps, Head2HeadRecordsState> {
    constructor(props: Head2HeadRecordsProps) {
        super(props);
        this.state = { records: [] };
    }

    public render() {
        return <div>
            <h2>Head 2 Head Records</h2>
            <table className="table table-condensed table-striped table-hover table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Opponent</th>
                        <th className="text-center">Games</th>
                        <th className="text-center">Wins</th>
                        <th className="text-center">Losses</th>
                        <th className="text-center">Pct</th>
                        <th className="text-center">Rating <small>(avg)</small></th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.records.map(h2h =>
                        <tr key={h2h.opponent}>
                            <td className="text-center">
                                <PlayerStatsLink player={h2h.opponent}/>
                            </td>
                            <td className="text-center">{h2h.gamesPlayed}</td>
                            <td className="text-center">{h2h.wins}</td>
                            <td className="text-center">{h2h.losses}</td>
                            <td className="text-center">{(100 * h2h.pct).toFixed(1)}</td>
                            <td className="text-center">
                                {this.getRatingChange(h2h.ratingChange)}
                                &nbsp;
                                {this.getRatingChangePerGame(h2h.ratingChange, h2h.gamesPlayed)}
                            </td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;
    }

    fetchRecords(props: Head2HeadRecordsProps) {
        if (props.player == '' || props.season == '') {
            return;
        }

        Api.getHead2HeadRecords(props.player, props.season)
            .then(data => this.setState({ records: data }));
    }

    getRatingChange(ratingChange: number) {
        var prefix = (ratingChange > 0 ? "+" : "");
        var textClass = (ratingChange > 0 ? "text-success" : (ratingChange < 0 ? "text-danger" : ""));

        return <span className={textClass}>{prefix}{ratingChange}</span>;
    }

    getRatingChangePerGame(ratingChange: number, gamesPlayed: number) {
        var ratingChangePerGame = ratingChange / gamesPlayed;

        return <small>(<span>{ratingChangePerGame.toFixed(1)}</span>)</small>;
    }

    componentWillMount() {
        this.fetchRecords(this.props);
    }

    componentWillReceiveProps(nextProps: Readonly<Head2HeadRecordsProps>) {
        if (this.props.player != nextProps.player || this.props.season != nextProps.season) {
            this.setState({ records: [] });
            this.fetchRecords(nextProps);
        }
    }
}