import * as React from 'react'
import { RouteComponentProps } from 'react-router-dom'
import { LatestGamesTable } from './LatestGamesTable';

export class GamesPage extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <div>
                <h1>Latest Games</h1>
            </div>
            <LatestGamesTable numGames={25}/>
        </div>;
    }
}