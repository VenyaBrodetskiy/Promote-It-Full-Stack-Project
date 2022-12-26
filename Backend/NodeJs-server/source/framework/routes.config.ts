import { Application } from "express"

export abstract class RouteConfig {
    private _app: Application;
    private _name: string;
    private _baseUrl: string;

    constructor(app: Application, name: string, baseUrl: string) {
        this._app = app;
        this._name = name;
        this._baseUrl = baseUrl;
        this.configureRoutes();
    }

    protected get app(): Application {
        return this._app;
    }

    protected get baseUrl(): string {
        return this._baseUrl;
    }

    public getName() {
        return this._name;
    }

    abstract configureRoutes(): Application;
}