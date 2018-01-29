import * as React from 'react'

interface LastUpdateTextProps {
    timestamp: Date;
}

export class LastUpdateText extends React.Component<LastUpdateTextProps, {}> {
    public render() {
        return <small>Last update: {this.props.timestamp.toLocaleString()}</small>;
    }
}
