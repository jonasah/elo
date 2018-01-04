import * as React from 'react';
import { RouteComponentProps } from 'react-router';
import { GameResultForm } from './GameResultForm';
import { PlayerRatingsTable } from './PlayerRatingsTable';

export class Home extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <h1>Player ratings</h1>
            <div>
                <GameResultForm />
            </div>
            <div>
                <PlayerRatingsTable/>
            </div>
        </div>;
    }
}
