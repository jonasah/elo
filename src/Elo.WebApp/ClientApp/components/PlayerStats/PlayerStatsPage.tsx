import * as React from 'react'
import { RouteComponentProps, Route } from 'react-router';
import { PlayerStats } from './PlayerStats';
import { PlayerStatsLink } from '../Common/PlayerStatsLink';
import * as Api from '../../api';

interface PlayerStatsPageState {
    players: string[];
}

export class PlayerStatsPage extends React.Component<RouteComponentProps<{}>, PlayerStatsPageState> {
    constructor(props: RouteComponentProps<{}>) {
        super(props);

        this.state = { players: [] };
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

            <Route path={`${this.props.match.url}/:player`} component={PlayerStats} />
        </div>;
    }

    componentWillMount() {
        Api.getPlayers().then(data => this.setState({ players: data }));
    }
}
