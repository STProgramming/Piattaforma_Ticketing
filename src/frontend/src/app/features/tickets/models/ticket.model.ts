export interface AzureDevOpsLinkRequest {
  createUserStory: boolean;
  title?: string | null;
  description?: string | null;
  areaPath?: string | null;
  iterationPath?: string | null;
  assignedTo?: string | null;
}

export interface Ticket {
  id: string;
  title: string;
  description: string;
  status: string;
  createdBy: string;
  createdAt: string;
  lastUpdatedAt?: string | null;
  azureDevOpsWorkItemId?: number | null;
  azureDevOpsWorkItemUrl?: string | null;
}

export interface CreateTicketRequest {
  title: string;
  description: string;
  createdBy: string;
  azureDevOps?: AzureDevOpsLinkRequest | null;
}
