import * as React from 'react';
import { Link } from 'react-router-dom';

interface Head2HeadRecordDto {
    opponent: string;
    gamesPlayed: number;
    wins: number;
    losses: number;
    pct: number;
}

interface Head2HeadRecordsProps {
    player: string;
}

interface Head2HeadRecordsState {
    records: Head2HeadRecordDto[];
}

export class Head2HeadRecords extends React.Component<Head2HeadRecordsProps, Head2HeadRecordsState> {
    constructor(props: Head2HeadRecordsProps) {
        super(props);
        this.state = { records: [] };

        fetch('api/elo/playerstats/' + this.props.player + '/h2h')
            .then(response => response.json() as Promise<Head2HeadRecordDto[]>)
            .then(data => this.setState({ records: data }));
    }

    public render() {
        return <div>
            <div className="page-header">
                <h2>Head 2 Head Records</h2>
            </div>
            <table className="table table-condensed table-striped table-hover table-bordered">
                <thead>
                    <tr>
                        <th className="text-center">Opponent</th>
                        <th className="text-center">Games</th>
                        <th className="text-center">Wins</th>
                        <th className="text-center">Losses</th>
                        <th className="text-center">Pct</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.records.map(h2h =>
                        <tr key={h2h.opponent}>
                            <td className="text-center">
                                <Link to={'/playerstats/' + h2h.opponent}>
                                    {h2h.opponent}
                                </Link>
                            </td>
                            <td className="text-center">{h2h.gamesPlayed}</td>
                            <td className="text-center">{h2h.wins}</td>
                            <td className="text-center">{h2h.losses}</td>
                            <td className="text-center">{(100 * h2h.pct).toFixed(1)}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>;
    }
}