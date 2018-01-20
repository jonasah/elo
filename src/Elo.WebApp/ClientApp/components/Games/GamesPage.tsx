import * as React from 'react'
import { RouteComponentProps } from 'react-router-dom'
import { LatestGames } from './LatestGames';

export class GamesPage extends React.Component<RouteComponentProps<{}>, {}> {
    public render() {
        return <LatestGames numGames={25}/>;
    }
}