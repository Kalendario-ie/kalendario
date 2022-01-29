
export interface IReadModel {
    id: string;
    name: string;
}

export interface Person extends IReadModel {
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
}

export function modelId(model: IReadModel) {
    if (model) {
        return model.id;
    }
    return null;
}

