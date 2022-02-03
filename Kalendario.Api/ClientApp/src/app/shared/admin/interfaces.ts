import {useState} from 'react';
import {ApiValidationError} from 'src/app/api/common/api-errors';
import {BaseModelRequestPostPut} from 'src/app/api/common/clients/base-django-api';
import {IReadModel} from 'src/app/api/common/models';

export interface AdminFormProps<TEntity> {
    entity: TEntity | null;
    onCancel: () => void;
    onSuccess: (entity: TEntity) => void;
}

export interface AdminTableContainerProps<TModel> {
    entities: TModel[];
    filter?: (value: string | undefined) => void;
    buttonsColumn: any
}

interface HandleSubmitResult<TUpsertCommand> {
    apiError: ApiValidationError | null;
    handleSubmit: (command: TUpsertCommand) => Promise<any>
}

export function useHandleSubmit<TResourceModel extends IReadModel, TUpsertCommand>(
    client: BaseModelRequestPostPut<TResourceModel, TUpsertCommand>,
    entity: TResourceModel | null,
    onSuccess: (entity: TResourceModel) => void,
): HandleSubmitResult<TUpsertCommand> {
    const [apiError, setApiError] = useState<ApiValidationError | null>(null);

    const handleSubmit = (command: TUpsertCommand): Promise<any> => {
        setApiError(null);

        return entity?.id
            ? client.put(entity.id, command)
                .then(entity => {
                    onSuccess(entity);
                }).catch(error => setApiError(error))
            : client.post(command)
                .then(entity => {
                    onSuccess(entity);
                }).catch(error => setApiError(error))
    }

    return {apiError, handleSubmit};
}
