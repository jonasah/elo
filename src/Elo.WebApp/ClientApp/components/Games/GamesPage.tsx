import * as React from 'react'
import { RouteComponentProps } from 'react-router-dom'
import { GamesTable } from './GamesTable';

export class GamesPage extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <div>
            <div>
                <h1>Latest Games</h1>
            </div>
            <GamesTable/>
        </div>;
    }
}